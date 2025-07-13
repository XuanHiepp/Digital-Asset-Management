<?php
define('ROOT', 'C:\xampp\htdocs\hdwallets');
require_once(ROOT. '/vendor/autoload.php');

if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
    $file_name = $_FILES["file_split"]["tmp_name"];
    $array = explode('.', $_FILES['file_split']['name']);
	$extension = end($array);
    $cut_from = $_POST["cut_from"];
    $duration = $_POST["duration"];

    $command = "C:/ffmpeg/bin/ffmpeg.exe -i " . $file_name . " -vcodec copy -ss " . $cut_from . " -t " . $duration . " output." . $extension;
	system($command);

	header("Location:http://localhost:8080/hdwallets/AssetCopyRight/AssetCopyRight_Success.php");	
}

