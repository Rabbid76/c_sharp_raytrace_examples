import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-rt1_in_one_weekend',
  templateUrl: './rt1_in_one_weekend.component.html',
})
export class Rt1InOneWeekendComponent {
  public raytrace: Rt1InOneWeekend;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Rt1InOneWeekend>(baseUrl + 'rt1inoneweekend').subscribe(result => {
      this.raytrace = result;
    }, error => console.error(error));
  }
}

interface Rt1InOneWeekend {
  title: string;
}
