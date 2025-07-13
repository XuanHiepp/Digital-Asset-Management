<?php
/**
 * index.php - The Entry File
**/

// The filename... You can get that from a $_GET variable and store it here

if (isset($_GET['token']))
{
	$token = base64_decode($_GET['token']);
	$encrypt_method = "AES-256-CBC";
	$secret_key = 'WS-SERVICE-KEY';
	$secret_iv = 'WS-SERVICE-VALUE';
	// hash
	$key = hash('sha256', $secret_key);

	// iv - encrypt method AES-256-CBC expects 16 bytes - else you will get a warning
	$iv = substr(hash('sha256', $secret_iv), 0, 16);

	// We will be encrypting the video name using session id as key ans AES128 as the algorithm
	$token_encrypted = openssl_encrypt($token, $encrypt_method, $key, 0, $iv);

	$ciphertext = base64_encode($token_encrypted);
}

?>

<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <title>Play Audio</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css">
	<link rel="stylesheet" href="AssetCopyRight.css">
	<link rel="stylesheet" href="style.css">
	<link rel="stylesheet" href="play_audio.css">
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
						<div class="container-audio">
	                     	<audio controls loop controlsList="nodownload" autoplay>
	                        	<source src="audio.php?vid=<?php echo $ciphertext; ?>&key=<?php echo $key; ?>&iv=<?php echo $iv; ?>" type="audio/ogg">
	                        	Your browser dose not Support the audio Tag
	                     	</audio>
	                  	</div>
	                  	<div class="container-audio">
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
		                     <div class="colum1">
		                        <div class="row"></div>
		                     </div>
	                  	</div>
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
  </body>
</html>
