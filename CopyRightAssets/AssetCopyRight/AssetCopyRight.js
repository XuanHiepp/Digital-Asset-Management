$(document).ready(function(){
  $(document).on('change', '#digital-music-type', function(e) {
    // $("#amount_input").val($(this).val());
    var digital_type = $(this).val();
    $(".track .step:nth-child(2)").addClass('active');
    if (parseInt(digital_type) == 0)
    {
          $(".lyrics-type").css("display","block");
          $(".audio-type").css("display","none");
          $(".video-type").css("display","none");
    }
    else if (parseInt(digital_type) == 1)
    {
          $(".lyrics-type").css("display","none");
          $(".audio-type").css("display","block");
          $(".video-type").css("display","none");
    }
    else if (parseInt(digital_type) == 2)
    {
          $(".lyrics-type").css("display","none");
          $(".audio-type").css("display","none");
          $(".video-type").css("display","block");
    }
  });
});