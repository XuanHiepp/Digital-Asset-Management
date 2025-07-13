<?php


require_once(__DIR__. '/vendor/getID3/getid3/getid3.php');

require_once(__DIR__. '/vendor/autoload.php');

// Initialize getID3 engine
$getID3 = new getID3;

$filename = "HaruHaru.mp3";


// Analyze file and store returned data in $ThisFileInfo
$ThisFileInfo = $getID3->analyze($filename);

/*
 Optional: copies data from all subarrays of [tags] into [comments] so
 metadata is all available in one location for all tag formats
 metainformation is always available under [tags] even if this is not called
*/
$getID3->CopyTagsToComments($ThisFileInfo);

/*
 Output desired information in whatever format you want
 Note: all entries in [comments] or [tags] are arrays of strings
 See structure.txt for information on what information is available where
 or check out the output of /demos/demo.browse.php for a particular file
 to see the full detail of what information is returned where in the array
 Note: all array keys may not always exist, you may want to check with isset()
 or empty() before deciding what to output
*/

// echo $ThisFileInfo['mime_type']; // artist from any/all available tag formats

echo $ThisFileInfo['id3v2']['UFID'][0]['ownerid'];  // title from ID3v2
//echo $ThisFileInfo['audio']['bitrate'];           // audio bitrate
//echo $ThisFileInfo['playtime_string'];            // playtime in minutes:seconds, formatted string

/* if you want to see all the tag data (from all tag formats), uncomment this line: */
//echo '<pre>'.htmlentities(print_r($ThisFileInfo['comments'], true), ENT_SUBSTITUTE).'</pre>';

/* if you want to see ALL the output, uncomment this line: */
// echo '<pre>'.htmlentities(print_r($ThisFileInfo, true), ENT_SUBSTITUTE).'</pre>';

