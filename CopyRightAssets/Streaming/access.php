<?php
/**
 * access.php - The main serving file which will server the video
**/
// session_start();

// $ivlen = openssl_cipher_iv_length($cipher="AES-128-CBC");
// $iv = openssl_random_pseudo_bytes($ivlen);
// // Decrypt the Token to get back the video file name
// $token = openssl_decrypt($_GET['vid'], $cipher, $_GET['id'].session_id());

$vid = $_GET['vid'];

// Check if file exists
$file = $vid;
$head = array_change_key_case(get_headers($file, TRUE));
$size = $head['content-length'];
header('Content-Type: audio/ogg');
header('Accept-Ranges: bytes');
header('Content-Disposition: inline');
header('Content-Length:'.$size);
readfile($file);

