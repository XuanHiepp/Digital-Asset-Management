import { Component, OnInit } from '@angular/core';
declare const loadVideo: any;
@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    loadVideo();
  }

}
