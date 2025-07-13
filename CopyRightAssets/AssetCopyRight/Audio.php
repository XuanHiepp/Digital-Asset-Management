<?php
define('ROOT', 'C:\xampp\htdocs\hdwallets');

require_once(ROOT. '/vendor/getID3/getid3/getid3.php');

require_once(ROOT. '/vendor/getID3/getid3/write.php');

require_once(ROOT. '/vendor/autoload.php');

$TaggingFormat = 'UTF-8';

header('Content-Type: text/html; charset='.$TaggingFormat);

if ($_SERVER['REQUEST_METHOD'] == 'POST')
{
	// Initialize getID3 engine
	$getID3 = new getID3;
	$getID3->setOption(array('encoding'=>$TaggingFormat));

	$source = "source";
	$destination = "destination";
	$filename = $_FILES["audio-file"]["name"];
	$text = $_POST['audio-text-logo'];

	// Initialize getID3 tag-writing module
	$tagwriter = new getid3_writetags;
	
	$tagwriter->filename = ROOT.'/'.$filename;

	//$tagwriter->tagformats = array('id3v1', 'id3v2.3');
	$tagwriter->tagformats = array('id3v1', 'id3v2.3');

	// set various options (optional)
	$tagwriter->overwrite_tags    = true;  
	$tagwriter->remove_other_tags = false; 
	$tagwriter->tag_encoding      = $TaggingFormat;
	$tagwriter->remove_other_tags = true;

	$TagData = array(
		'title'                  => array($_POST['title']),
		'artist'                 => array($_POST['artist']),
		'album'                  => array($_POST['album']),
		'year'                   => array($_POST['year']),
		'genre'                  => array($_POST['genre']),
		'comment'                => array($_POST['comment']),
		'track_number'           => array('04/16'),
		'popularimeter'          => array('email'=>$_POST['popularimeter'], 'rating'=>128, 'data'=>0),
		'unique_file_identifier' => array('ownerid'=>$_POST['unique_file_identifier'], 'data'=>md5(time())),
	);

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

	$TagData['attached_picture'][0]['data']          = file_get_contents($source.'/'.$name.".jpg", true);
	$TagData['attached_picture'][0]['picturetypeid'] = 2;
	$TagData['attached_picture'][0]['description']   = 'thumnail';
	$TagData['attached_picture'][0]['mime']          = 'image/jpeg';
	$tagwriter->tag_data = $TagData;

	// write tags
	if ($tagwriter->WriteTags()) {
		header("Location:http://localhost:8080/hdwallets/AssetCopyRight/AssetCopyRight_Success.php");	
	} else {
		echo 'Failed to write tags!<br>'.implode('<br><br>', $tagwriter->errors);
	}
}
