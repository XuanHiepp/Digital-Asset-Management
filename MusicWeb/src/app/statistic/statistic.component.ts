import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { MusicItem } from '../shared/music-item.model';
import { MusicService } from '../shared/music.service';
import { ToastrService } from "ngx-toastr";

@Component({
  selector: 'app-statistic',
  templateUrl: './statistic.component.html',
  styleUrls: ['./statistic.component.css']
})
export class StatisticComponent implements OnInit {
  public apiURL = "https://api-ropsten.etherscan.io/api?module=proxy";
  readonly apiURLExpress = "https://localhost:5001/api";
  blockNumber: String;
  totalTransact: String;
  convert: ConvertModel = new ConvertModel();
  valueGasPriceConvert: ConvertGasPricePModel = new ConvertGasPricePModel();
  trasactModel: TrasactModel = new TrasactModel();
  items = [];
  public musicItemList: MusicItem[];
  public userID = localStorage.getItem("UserID");
  constructor(private http:HttpClient,
    public service: MusicService,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.getLatestBlock();
    this.service.getMusicList().then(res => this.musicItemList = res as MusicItem[]);
    var refresh = window.localStorage.getItem('refresh');
    // console.log(refresh);
    if (refresh===null){
        window.location.reload();
        window.localStorage.setItem('refresh', "1");
    }
  }

  searchOwnerShip(searchInfo)
  {
    if (searchInfo["searchHash"] == "")
    {
      this.toastr.error("Thông tin bị thiếu ...", "Error", {
        positionClass: "toast-top-right",
        timeOut: 3000
      });
    }
    else
    {
      this.toastr.warning("Đang tìm kiếm thông tin ...", "Warning", {
        positionClass: "toast-top-right",
        timeOut: 3000
      });
      setTimeout(() => 
      {
        this.toastr.success("Tìm kiếm thành công, đang chuyển trang...", "Success", {
          positionClass: "toast-top-right",
          timeOut: 3000
        });
      },
      3300);
      
      setTimeout(() => 
      {
        this.router.navigate(['/music/origin-music/'+searchInfo["searchHash"]]);
      },
      6000);
    }
  }

  getLatestBlock()
  {
    this.http.get(this.apiURL + '&action=eth_blockNumber&apikey=KKHEWWF9KJ9189V5G3W79TT7FZB79E3PJ9').subscribe(res =>{
      this.convert.blockNumber = res['result'];
      // this.getCountTransactionIntoBlock(this.convert.blockNumber);
      this.getTransactionIntoBlockWithIndex(this.convert.blockNumber);
      this.http.post(this.apiURLExpress + '/Statistics', this.convert)
      .subscribe(res =>{
        this.convert.blockNumber = res['blockHeight'];
        // console.log(this.convert.blockNumber);
      });
    });
  }

  // getCountTransactionIntoBlock(blockNumber?: String | null | undefined)
  // {
  //   this.http.get(this.apiURL + '&action=eth_getBlockTransactionCountByNumber&tag='+blockNumber+'&apikey=KKHEWWF9KJ9189V5G3W79TT7FZB79E3PJ9').subscribe(res =>{
  //     this.convert.blockNumber = res['result'];
  //     this.http.post(this.apiURLExpress + '/Statistics', this.convert)
  //     .subscribe(res =>{
  //       this.totalTransact = res['blockHeight'];
  //     });
  //   });
  // }

  getTransactionIntoBlockWithIndex(blockNumber?: String | null | undefined)
  {
    for(let i = 0; i < 6; i++){
      this.http.get(this.apiURL + '&action=eth_getTransactionByBlockNumberAndIndex&tag='+blockNumber+'&index=0x'+i+'&apikey=KKHEWWF9KJ9189V5G3W79TT7FZB79E3PJ9')
        .subscribe(res =>{
          this.trasactModel.txId = res['result']['hash'];
          this.trasactModel.from = res['result']['from'];
          this.trasactModel.to = res['result']['to'];
          this.trasactModel.gas = res['result']['gas'];
          this.trasactModel.gasPrice = res['result']['gasPrice'];
          this.valueGasPriceConvert.blockNumber = this.trasactModel.gasPrice;
          this.trasactModel.txId = this.trasactModel.txId.substring(0, 35)+'...';
          this.trasactModel.from = this.trasactModel.from.substring(0, 16)+'...';
          this.trasactModel.to = this.trasactModel.to.substring(0, 16)+'...';
          this.items.push({
            txId: this.trasactModel.txId, txIdFull: res['result']['hash'], from: this.trasactModel.from, to: this.trasactModel.to,
            gasPrice: this.valueGasPriceConvert.blockNumber
          });

          this.http.post(this.apiURLExpress + '/Statistics', this.valueGasPriceConvert)
            .subscribe(res =>{
              this.valueGasPriceConvert.blockNumber = res['blockHeight'];
              this.items[i].gasPrice = Number(this.valueGasPriceConvert.blockNumber)/ (1000000000);
              let gasPrice = String(this.items[i].gasPrice);
              this.items[i].gasPrice = gasPrice.substring(0, 8);
            });
          
          // console.log(this.items);
        });
      
    }
    // console.log(this.items);
  }

  getMusicList()
  {
    this.service.getMusicList().then(res => this.musicItemList = res as MusicItem[]);
  }


}

export class ConvertModel {
  blockNumber: String;
}

export class ConvertGasPricePModel {
  blockNumber: String;
}

export class CounTrasactModel {
  totalTransact: String;
}

export class TrasactModel {
  txId: String;
  from: String;
  to: String;
  blockNumber: String;
  gas: String;
  gasPrice: String;
}


