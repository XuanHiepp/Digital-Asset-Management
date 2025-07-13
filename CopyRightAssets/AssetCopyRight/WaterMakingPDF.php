<?php

define('ROOT', 'C:\xampp\htdocs\hdwallets');

require_once(ROOT. '/vendor/autoload.php');
require_once(ROOT. '/vendor/setasign/fpdf/fpdf.php');
use setasign\Fpdi\Fpdi;
if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
	$file = $_FILES["lyrics"]["name"];
	$text = "Copy Right & Law By ".$_POST['copyrightLyrics']."!!";
	$op = 100;
	$outdir = TRUE;
	$source = "source";
	$destination = "destination";
    $name = uniqid();
    $font_size = 5;
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
    // Created Watermark Image
    $pdf = new FPDI();
    if (file_exists("./".$file)){
        $pagecount = $pdf->setSourceFile($file);
    } else {
        return FALSE;
    }
    $tpl = $pdf->importPage(1);
    $pdf->addPage();
    $pdf->useTemplate($tpl, 10, 10, 220, 400, TRUE);
    //Put the watermark
    $pdf->Image($source.'/'.$name.'.jpg', 130, 30, 0, 0, 'jpg');
    if ($outdir === TRUE){
        $pdf->Output($destination.'/'.'music-'.$name.'.pdf', 'F');
        header("Location:http://localhost:8080/hdwallets/AssetCopyRight/AssetCopyRight_Success.php");
    }
}


