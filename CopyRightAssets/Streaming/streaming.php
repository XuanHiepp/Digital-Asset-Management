<?php
require_once __DIR__. '/vendor/autoload.php';

use Streaming\HLS;
use Streaming\Representation;

$config = [
    'ffmpeg.binaries'  => 'C:/ffmpeg/bin/ffmpeg.exe',
    'ffprobe.binaries' => 'C:/ffmpeg/bin/ffprobe.exe',
    'timeout'          => 3600, // The timeout for the underlying process
    'ffmpeg.threads'   => 12,   // The number of threads that FFmpeg should use
];

    
$ffmpeg = Streaming\FFMpeg::create($config);
$video = $ffmpeg->open('C:/xampp/htdocs/hdwallets/sample.mp4');

$hls = $video->hls()
            ->x264()
            ->autoGenerateRepresentations();
            
$hls->setMasterPlaylist('https://videomp4-usea.streaming.media.azure.net//f470e4cb-4d9a-4d32-ac12-8fea02e1b431/sampleLarge.ism/manifest(format=m3u8-cmaf)');