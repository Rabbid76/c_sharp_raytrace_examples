import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-rt1_in_one_weekend',
  templateUrl: './rt1_in_one_weekend.component.html',
})
export class Rt1InOneWeekendComponent {
  public raytrace: Rt1InOneWeekend;
  public raytraceImage: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Rt1InOneWeekend>(baseUrl + 'rt1inoneweekend').subscribe(result => {
      this.raytrace = result;
      this.raytraceImage = "data:image/png;base64," + this.raytrace.imagePng;
      let imageelem: any = document.getElementById("raytraceimage");
      imageelem.src = this.raytraceImage;
    }, error => console.error(error));
  }
}

interface Rt1InOneWeekend {
  title: string;
  imagePng: string;
}
