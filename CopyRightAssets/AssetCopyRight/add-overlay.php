<?php
define('ROOT', 'C:\xampp\htdocs\hdwallets');
if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
	// first you have to get both input files in separate variables
	$video = $_FILES["video"]["name"];
	$text = $_POST['video-logo-text'];
	$source = "source";
	$destination = "destination";

	$name = uniqid();
    $font_size = 5;
    $op = 100;
    $ts=explode("\n",$text);
    $width=0;
    foreach ($ts as $k=>$string) {
        $width=max($width,strlen($string));
    }
    $width  = imagefontwidth($font_size)*$width;
    $height = imagefontheight($font_size)*count($ts);
    $el=imagefontheight($font_size);
    $em=imagefontwidth($font_size);
    $img = imagecreatetruecolor($width,$height);
    // Background color
    $bg = imagecolorallocate($img, 255, 255, 255);
    imagefilledrectangle($img, 0, 0,$width ,$height , $bg);
    // Font color
    $color = imagecolorallocate($img, 0, 0, 0);
    foreach ($ts as $k=>$string) {
        $len = strlen($string);
        $ypos = 0;
        for($i=0;$i<$len;$i++){
            $xpos = $i * $em;
            $ypos = $k * $el;
            imagechar($img, $font_size, $xpos, $ypos, $string, $color);
            $string = substr($string, 1);      
        }
    }
    imagecolortransparent($img, $bg);
    $blank = imagecreatetruecolor($width, $height);
    $tbg = imagecolorallocate($blank, 255, 255, 255);
    imagefilledrectangle($blank, 0, 0,$width ,$height , $tbg);
    imagecolortransparent($blank, $tbg);
    if ( ($op < 0) OR ($op >100) ){
        $op = 100;
    }
    imagecopymerge($blank, $img, 0, 0, 0, 0, $width, $height, $op);
    imagejpeg($blank,$source.'/'.$name.".jpg");
	 
	// both input files has been selected
	$command = "ffmpeg -i " .ROOT. '/' . $video . " -i ". ROOT. '/AssetCopyRight/'. $source . '/'.$name.".jpg";
	 
	// now apply the filter to select both files
	// it must enclose in double quotes
	// [0:v] means first input which is video
	// [1:v] means second input which is resized image
	$command .= " -filter_complex \"[0:v][1:v]";
	 
	// now we need to tell the position of overlay in video
	$command .= " overlay=80:50\""; // closing double quotes
	 
	// save in a separate output file
	$command .= " -c:a copy output_".$name.".mp4";
	 
	// execute the command
	system($command);
	 
	// echo "Overlay has been added";
    header("Location:http://localhost:8080/hdwallets/AssetCopyRight/AssetCopyRight_Split.php");    
}

 
