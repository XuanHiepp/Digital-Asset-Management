using Microsoft.Extensions.Options;
using MusicServer.Repositories.Interfaces;
using MusicServer.Services.Interfaces;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MusicServer.Models.Database.KPI;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Numerics;

namespace MusicServer.Services.Enforcements
{
    public class PKIEncordeService : IPKIEncordeService
    {
        private readonly IEthereumService ethereumService;
        private readonly IMusicRepository musicRepository;
        private readonly string MusicAbi;

        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string Csr { get; set; }

        public PKIEncordeService(
           IEthereumService ethereumService,
           IMusicRepository musicRepository,
           IOptions<EthereumSettings> options)
        {
            this.ethereumService = ethereumService;
            this.musicRepository = musicRepository;
            MusicAbi = options.Value.MusicAbi;
        }

        void IPKIEncordeService.GenerateGemFileForKPI(
                string commonName,
                string organization,
                string organizationalUnit,
                string locality,
                string state,
                string countryIso2Characters,
                string emailAddress,
                SignatureAlgorithm signatureAlgorithm,
                RsaKeyLength rsaKeyLength)
        {
            countryIso2Characters = "US";
            emailAddress = "";
            signatureAlgorithm = SignatureAlgorithm.SHA256;
            rsaKeyLength = RsaKeyLength.Length2048Bits;

            #region Determine Signature Algorithm

            string signatureAlgorithmStr;
            switch (signatureAlgorithm)
            {
                case SignatureAlgorithm.SHA1:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id;
                    break;

                case SignatureAlgorithm.SHA256:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id;
                    break;

                case SignatureAlgorithm.SHA512:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha512WithRsaEncryption.Id;
                    break;

                default:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id;
                    break;
            }

            #endregion

            #region Cert Info

            IDictionary attrs = new Hashtable();

            attrs.Add(X509Name.CN, commonName);
            attrs.Add(X509Name.O, organization);
            attrs.Add(X509Name.OU, organizationalUnit);
            attrs.Add(X509Name.L, locality);
            attrs.Add(X509Name.ST, state);
            attrs.Add(X509Name.C, countryIso2Characters);
            attrs.Add(X509Name.EmailAddress, emailAddress);

            X509Name subject = new X509Name(new ArrayList(attrs.Keys), attrs);

            #endregion

            #region Key Generator

            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), (int)rsaKeyLength));
            AsymmetricCipherKeyPair pair = rsaKeyPairGenerator.GenerateKeyPair();

            #endregion

            #region CSR Generator

            string path_project_bin = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));

            Asn1SignatureFactory signatureFactory = new Asn1SignatureFactory(signatureAlgorithmStr, pair.Private);

            Pkcs10CertificationRequest csr = new Pkcs10CertificationRequest(signatureFactory, subject, pair.Public, null, pair.Private);

            #endregion

            #region Convert to PEM and Output

            #region Private Key

            StringBuilder privateKeyStrBuilder = new StringBuilder();
            PemWriter privateKeyPemWriter = new PemWriter(new StringWriter(privateKeyStrBuilder));
            privateKeyPemWriter.WriteObject(pair.Private);
            privateKeyPemWriter.Writer.Flush();
            string pathToNewFolder = System.IO.Path.Combine(path_project_bin + "/bin/keys/users/", "2");
            DirectoryInfo directory = Directory.CreateDirectory(pathToNewFolder);
            File.WriteAllText(path_project_bin + "/bin/keys/users/2/private.pem", privateKeyStrBuilder.ToString());

            PrivateKey = privateKeyStrBuilder.ToString();

            #endregion Private Key

            #region Public Key

            StringBuilder publicKeyStrBuilder = new StringBuilder();
            PemWriter publicKeyPemWriter = new PemWriter(new StringWriter(publicKeyStrBuilder));
            publicKeyPemWriter.WriteObject(pair.Public);
            publicKeyPemWriter.Writer.Flush();
            
            File.WriteAllText(path_project_bin + "/bin/keys/users/2/public.pem", publicKeyStrBuilder.ToString());

            PublicKey = publicKeyStrBuilder.ToString();

            #endregion Public Key

            #region CSR


            StringBuilder csrStrBuilder = new StringBuilder();
            PemWriter csrPemWriter = new PemWriter(new StringWriter(csrStrBuilder));
            csrPemWriter.WriteObject(csr);
            csrPemWriter.Writer.Flush();
            File.WriteAllText(path_project_bin + "/bin/keys/users/2/publicCert.pem", csrStrBuilder.ToString());

            Csr = csrStrBuilder.ToString();

            #endregion CSR

            #endregion

        }

        // get key pair from two local files
        AsymmetricCipherKeyPair IPKIEncordeService.GetKeyPair(int keyType, int userID)
        {

            AsymmetricKeyParameter privateKey, publicKey;
            string path_project_bin = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));
            var privateKeyString = "";
            var certificateString = "";
            if (keyType == 0)
            {
                privateKeyString = File.ReadAllText(path_project_bin + "/bin/keys/server/private.pem");
                certificateString = File.ReadAllText(path_project_bin + "/bin/keys/server/publicCert.pem");
            }
            else if (keyType == 1)
            {
                if (userID != 0)
                {
                    privateKeyString = File.ReadAllText(path_project_bin + "/bin/keys/users/" + userID +"/private.pem");
                    certificateString = File.ReadAllText(path_project_bin + "/bin/keys/users/" + userID + "/publicCert.pem");
                }
            }
            
            using (var textReader = new StringReader(privateKeyString))
            {
                // Only a private key
                var pseudoKeyPair = (AsymmetricCipherKeyPair)new PemReader(textReader).ReadObject();
                privateKey = pseudoKeyPair.Private;
            }

            
            using (var textReader = new StringReader(certificateString))
            {
                // Only a private key
                Pkcs10CertificationRequest bcCertificate = (Pkcs10CertificationRequest)new PemReader(textReader).ReadObject();
                publicKey = bcCertificate.GetPublicKey();
            }

            return new AsymmetricCipherKeyPair(publicKey, privateKey);

        }

        byte[] IPKIEncordeService.SignData(byte[] data, AsymmetricKeyParameter privateKey)
        {
            var signer = SignerUtilities.GetSigner("SHA256withRSA");

            signer.Init(true, privateKey);

            signer.BlockUpdate(data, 0, data.Length);

            return signer.GenerateSignature();
        }

        bool IPKIEncordeService.VerifySignature(byte[] data, byte[] signature, AsymmetricKeyParameter publicKey)
        {

            var verifier = SignerUtilities.GetSigner("SHA256withRSA");

            verifier.Init(false, publicKey);

            verifier.BlockUpdate(data, 0, data.Length);

            return verifier.VerifySignature(signature);

        }

        bool IPKIEncordeService.TestSignAndVerify()
        {
            SHA256Managed sha256 = new SHA256Managed();
            string data = "Hello world - 3";
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            byte[] hashedMessage = sha256.ComputeHash(dataBytes);

            var keyPair = (this as IPKIEncordeService).GetKeyPair(0,1);

            var signature = (this as IPKIEncordeService).SignData(hashedMessage, keyPair.Private);

            var valid = (this as IPKIEncordeService).VerifySignature(hashedMessage, signature, keyPair.Public);

            return valid;
            //Console.WriteLine("Sign this message with the private key: " + data);
            //Console.WriteLine("and verify that the signature is okay with the public key.");

            ////Console.WriteLine("Signature: " + signature.Length);

            //Console.WriteLine(valid);
            //Console.ReadKey();

        }

        BigInteger IPKIEncordeService.RandomInteger(int length) // to generate a random number
        {
            Random random = new Random();
            byte[] data = new byte[length];
            random.NextBytes(data);
            return new BigInteger(data);
        }
    }
}
