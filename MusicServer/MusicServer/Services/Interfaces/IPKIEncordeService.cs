using MusicServer.Models.Database.KPI;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MusicServer.Services.Interfaces
{
    public interface IPKIEncordeService
    {
        void GenerateGemFileForKPI(string commonName,
                string organization,
                string organizationalUnit,
                string locality,
                string state,
                string countryIso2Characters,
                string emailAddress,
                Models.Database.KPI.SignatureAlgorithm signatureAlgorithm,
                RsaKeyLength rsaKeyLength);

        AsymmetricCipherKeyPair GetKeyPair(int keyType, int userID);

        byte[] SignData(byte[] data, AsymmetricKeyParameter privateKey);

        bool VerifySignature(byte[] data, byte[] signature, AsymmetricKeyParameter publicKey);

        bool TestSignAndVerify();

        BigInteger RandomInteger(int length);

    }
}
