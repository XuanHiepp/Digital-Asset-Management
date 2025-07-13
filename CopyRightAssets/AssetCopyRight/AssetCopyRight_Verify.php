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
	                <div class="step active"> <span class="icon"> <i class="fa fa-check"></i> </span> <span class="text"> Upload nhạc số và gán bản quyền </span> </div>
	                <div class="step active"> <span class="icon"> <i class="fa fa-user"></i> </span> <span class="text"> In vết bản quyền </span> </div>
	                <div class="step active"> <span class="icon"> <i class="fa fa-user-check"></i> </span> <span class="text"> Kiểm tra kết quả </span> </div>
	                <div class="step"> <span class="icon"> <i class="fa fa-box"></i> </span> <span class="text"> Xuất file mã hóa </span> </div>
	            </div>
	            <hr>
	            <ul class="row">
	                <li class="col-md-12">
						<div class="w-100 btn btn-success text-white">
							<h5  style="text-align: center;">Xác nhận thông tin đã in vết</h5>
					  		<h6  style="text-align: center;">Đảm bảo mọi thông tin bản quyền được in vết khớp với thông tin đã khai báo !!</h6>
						</div>
	                </li>
	            </ul>
	            <hr>
	            <a href="http://localhost/hdwallets/AssetCopyRight/AssetCopyRight.php" class="btn btn-danger" data-abc="true" style="float: left;"><i class="fa fa-chevron-left"> Previous Step </i></a>
	            <a href="http://localhost/hdwallets/AssetCopyRight/AssetCopyRight_Encrypt.php" class="btn btn-warning" data-abc="true" style="float: right;"> Next Step <i class="fa fa-chevron-right"></i></a>
	        </div>
	    </article>
	</div>
	<?php 
		define('ROOT', 'C:\xampp\htdocs\hdwallets');
		require_once(ROOT. '/AssetCopyRight/Footer.php');
	?>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script src="AssetCopyRight.js"></script> 
</body>
</html>

