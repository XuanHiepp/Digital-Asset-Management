<?php
/**
 * video.php - The First Entry Point
**/

// Get Token
$ciphertext = $_GET['vid'];
$key = $_GET['key'];
$iv = $_GET['iv'];

$encrypt_method = "AES-256-CBC";
$ciphertext_raw = base64_decode($ciphertext);

// Now we will re-encrypt the token
$original_plaintext = openssl_decrypt($ciphertext_raw, $encrypt_method, $key, 0, $iv);

header("Location: access.php?vid=".$original_plaintext);
// echo $original_plaintext;
?>