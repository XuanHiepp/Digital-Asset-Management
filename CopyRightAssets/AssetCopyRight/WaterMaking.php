<?php

require_once(__DIR__. '/vendor/autoload.php');

$source = "source";
$destination = "destination";
$watermake = "watermake/";

$watermark_emmbed = imagecreatefrompng($watermake.'watermake.png');

$margin_right = 10;
$margin_bottom = 10;
$sx = imagesx($watermark_emmbed);
$sy = imagesy($watermark_emmbed);

$img = imagecreatefromjpeg($source.'/'. 'background.jpg');

imagecopy($img, $watermark_emmbed, imagesx($img) - $sx - $margin_right, imagesy($img) - $sy - $margin_bottom, 0, 0, $sx, $sy);


$i = imagejpeg($img, $destination.'/'.'logo.jpg', 100);

imagedestroy($img);

// require_once(__DIR__. '/Audio.php');


