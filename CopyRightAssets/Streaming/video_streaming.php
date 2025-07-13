<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>Streaming Video</title>
    <link href="video-js.css" rel="stylesheet">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css">
	<link rel="stylesheet" href="AssetCopyRight.css">
	<link rel="stylesheet" href="style.css">
	<style>
		.maat-player-dimensions{
			width: 500px;
		    height: 300px;
		    margin: 0 auto;
		}
	</style>
  </head>
  <body>
  	<?php 
		define('ROOT', 'C:\xampp\htdocs\hdwallets');
		require_once(ROOT. '/AssetCopyRight/Header.php');
	?>
	<div class="container contain-info">
	    <article class="card">
	        <header class="card-header"> Thông tin tác phẩm </header>
	        <div class="card-body">
	            <h6>Mã nhạc số: DAM5345435</h6>
	            <article class="card">
	                <div class="card-body row">
	                    <div class="col"> <strong>File đính kèm</strong> <br>Lyrics </div>
	                    <div class="col"> <strong>Tên tác phẩm</strong> <br>29 nov 2019 </div>
	                    <div class="col"> <strong>Sở hữu bởi:</strong> <br> BLUEDART, | <i class="fa fa-phone"></i> +1598675986 </div>
	                    <div class="col"> <strong>Đăng tải</strong> <br> Picked by the courier </div>
	                    <div class="col"> <strong>Trị giá gốc</strong> <br> 200,000 VNĐ </div>
	                </div>
	            </article>
	            <article class="card">
	                <div class="card-body row">
	                        <video id="maat-player" class="video-js vjs-default-skin" controls>
						      <source src="<?php if (isset($_GET['mediaLink'])) { echo base64_decode($_GET['mediaLink']); }?>(format=mpd-time-cmaf)" type="application/dash+xml">
						    </video>
	                </div>
	            </article>
	        </div>
	    </article>
	 </div>
	<?php 
		require_once(ROOT. '/AssetCopyRight/Footer.php');
	?>
	<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
	<script src="AssetCopyRight.js"></script> 
    <script type="text/javascript" src="video.core.js"></script>
    <script type="text/javascript" src="videojs-http-streaming.js"></script>
    <script>
      (function(window, videojs) {
        var player = window.player = videojs('maat-player');
        var audioTrackList = player.audioTracks();
        var audioTrackSelect = document.getElementById("enabled-audio-track");
        // watch for a change on the select element
        // then change the enabled audio track
        // only one can be enabled at a time, but video.js will
        // handle that for us, all we need to do is enable the new
        // track
        audioTrackSelect.addEventListener('change', function() {
          var track = audioTrackList[this.selectedIndex];
          console.log('User switched to track ' + track.label);
          track.enabled = true;
        });

        // watch for changes that will be triggered by any change
        // to enabled on any audio track. Manually or through the
        // select element
        audioTrackList.on('change', function() {
          for (var i = 0; i < audioTrackList.length; i++) {
            var track = audioTrackList[i];
            if (track.enabled) {
              console.log('A new ' + track.label + ' has been enabled!');
            }
          }
        });

        // will be fired twice in this example
        audioTrackList.on('addtrack', function() {
          console.log('a track has been added to the audio track list');
        });

        // will not be fired at all unless you call
        // audioTrackList.removeTrack(trackObj)
        // we typically will not need to do this unless we have to load
        // another video for some reason
        audioTrackList.on('removetrack', function() {
          console.log('a track has been removed from the audio track list');
        });

        // getting all the possible audio tracks from the track list
        // get all of thier properties
        // add each track to the select on the page
        // this is all filled out by HLS when it parses the m3u8
        player.on('loadeddata', function() {
          console.log('There are ' + audioTrackList.length + ' audio tracks');
          for (var i = 0; i < audioTrackList.length; i++) {
            var track = audioTrackList[i];
            var option = document.createElement("option");
            option.text = track.label;
            if (track.enabled) {
              option.selected = true;
            }
            audioTrackSelect.add(option, i);
            console.log('Track ' + (i + 1));
            ['label', 'enabled', 'language', 'id', 'kind'].forEach(function(prop) {
              console.log("  " + prop + ": " + track[prop]);
            });
          }
        });
      }(window, window.videojs));
    </script>
  </body>
</html>
