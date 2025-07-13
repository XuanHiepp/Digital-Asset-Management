<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="UTF-8">
	<title>Document</title>
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css">
	<link rel="stylesheet" href="AssetCopyRight.css">
	<link rel="stylesheet" href="style.css">
</head>
<body>
	<?php 
		define('ROOT', 'C:\xampp\htdocs\hdwallets');
		require_once(ROOT. '/AssetCopyRight/Header.php');
	?>
	<div class="container">
	    <article class="card">
	        <header class="card-header"> Quản lý bản quyền </header>
	        <div class="card-body">
	            <h6>Mã nhạc số: DAM5345435</h6>
	            <article class="card">
	                <div class="card-body row">
	                    <div class="col"> <strong>Ngày tạo:</strong> <br>29 nov 2019 </div>
	                    <div class="col"> <strong>Sở hữu bởi:</strong> <br> BLUEDART, | <i class="fa fa-phone"></i> +1598675986 </div>
	                    <div class="col"> <strong>Trạng thái:</strong> <br> Picked by the courier </div>
	                    <div class="col"> <strong>Người tạo:</strong> <br> BLUEDART </div>
	                </div>
	            </article>
	            <div class="track">
	                <div class="step active"> <span class="icon"> <i class="fa fa-check"></i> </span> <span class="text"> Upload nhạc số </span> </div>
	                <div class="step"> <span class="icon"> <i class="fa fa-user"></i> </span> <span class="text"> In vết bản quyền </span> </div>
	                <div class="step"> <span class="icon"> <i class="fa fa-truck"></i> </span> <span class="text"> Kiểm tra kết quả </span> </div>
	                <div class="step"> <span class="icon"> <i class="fa fa-box"></i> </span> <span class="text"> Xuất file mã hóa </span> </div>
	            </div>
	            <hr>
	            <ul class="row">
	                <li class="col-md-12">
                    	<div class="form-group">
						  	<label for="digital-music-type">Nhạc số:</label>
						  	<select class="form-control" id="digital-music-type">
						    	<option disabled="disabled">Chọn thể loại nhạc số</option>
						    	<option value="0">Lyrics</option>
						    	<option value="1">Audio</option>
						    	<option value="2">Video</option>
						  </select>
						</div>
						<br>
					  	<div class="lyrics-type">
					  		<h4 style="text-align: center;">Cập nhật thông tin File Audio</h4>
					  		<div style="" class="col-md-12">
					  			<form method="POST" enctype="multipart/form-data" action="WaterMakingPDF.php">
						  			<div class="form-group">
								    	<label for="lyrics">Lyrics:</label>
								    	<input type="file" name="lyrics" class="form-control" id="lyrics">
								  	</div>
								  	<div class="form-group">
								    	<label for="text-logo">Thông tin bản quyền:</label>
								    	<input type="text" name="copyrightLyrics" class="form-control" placeholder="Enter text in logo" id="text-logo">
								  	</div>
								  	<input type="submit" class="btn btn-success" value="Xác nhận">
						  		</form>
					  		</div>
					  	</div>
					  	<div class="audio-type">
					  		<h4 style="text-align: center;">Cập nhật thông tin File Audio</h4>
							<div style="" class="col-md-12">
								<form method="POST" enctype="multipart/form-data" action="Audio.php" >
									<h3 style="color: gray;">Thông tin bài hát:</h3>
									<div class="row">
										<div class="col-md-12">
							  				<div class="form-group">
										    	<label for="audio">Audio File:</label>
										    	<input type="file" name="audio-file" class="form-control" id="audio">
										  	</div>
							  			</div>
									</div>
					  				<div class="row">
					  					<div class="col-md-6">
							  				<div class="form-group">
										    	<label for="title">Tiêu đề bài hát:</label>
										    	<input type="text" name="title" class="form-control" placeholder="Enter title" id="title">
										  	</div>
										  	<div class="form-group">
										    	<label for="album">Album:</label>
										    	<input type="text" name="album" class="form-control" placeholder="Enter album" id="album">
										  	</div>
										  	<div class="form-group">
										    	<label for="genre">Genre:</label>
										    	<input type="text" name="genre" class="form-control" placeholder="Enter genre" id="genre">
										  	</div>
							  			</div>
							  			<div class="col-md-6">
							  				<div class="form-group">
										    	<label for="artist">Artist:</label>
										    	<input type="text" name="artist" class="form-control" placeholder="Enter artist" id="artist">
										  	</div>
										  	<div class="form-group">
										    	<label for="year">year:</label>
										    	<input type="text" name="year" class="form-control" placeholder="Enter year" id="year">
										  	</div>
										  	<div class="form-group">
										    	<label for="comment">Đánh giá:</label>
										    	<input type="text" name="comment" class="form-control" placeholder="Enter comment" id="comment">
										  	</div>
							  			</div>	
					  				</div>
					  				<br>
					  				<h3 style="color: gray;">Thông tin bản quyền:</h3>
									<div class="row">
					  					<div class="col-md-6">
							  				<div class="form-group">
										    	<label for="popularimeter">Popularimeter:</label>
										    	<input type="text" name="popularimeter" class="form-control" placeholder="Enter identifier" id="popularimeter">
										  	</div>
							  			</div>
							  			<div class="col-md-6">
							  				<div class="form-group">
										    	<label for="unique_file_identifier">Unique File Identifier:</label>
										    	<input type="text" name="unique_file_identifier" class="form-control" placeholder="Enter identifier" id="unique_file_identifier">
										  	</div>
							  			</div>	
							  			<div class="col-md-6">
							  				<div class="form-group">
										    	<label for="text-logo">Thông tin bản quyền:</label>
								    			<input type="text" name="audio-text-logo"  class="form-control" placeholder="Enter text in logo" id="text-logo">
										  	</div>
							  			</div>	
					  				</div>
					  				<input type="submit" class="btn btn-success" value="Xác nhận">
						  		</form>
							</div>
					  	</div>
					  	<div class="video-type">
					  		<h4 style="text-align: center;">Cập nhật thông tin File Video</h4>
					  		<div style="" class="col-md-12">
					  			<form method="POST" enctype="multipart/form-data" action="add-overlay.php">
						  			<div class="form-group">
								    	<label for="video">Video:</label>
								    	<input type="file" name="video" class="form-control" id="video">
								  	</div>
								  	<div class="form-group">
								    	<label for="text-logo">Thông tin bản quyền:</label>
								    	<input type="text" name="video-logo-text" class="form-control" placeholder="Enter text in logo" id="text-logo">
								  	</div>
								  	<input type="submit" class="btn btn-success" value="Xác nhận">
						  		</form>
					  		</div>
					  	</div>
	                </li>
	            </ul>
	            <hr>
	            <a href="#" class="btn btn-warning" data-abc="true" style="float: right;"> Next step <i class="fa fa-chevron-right"></i></a>
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

