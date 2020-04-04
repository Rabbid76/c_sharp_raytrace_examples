import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Rt1InOneWeekend } from '../services/model/rt1_models'

@Component({
  selector: 'app-rt1_in_one_weekend',
  templateUrl: './rt1_in_one_weekend.component.html',
})
export class Rt1InOneWeekendComponent implements OnInit {
  public raytrace: Rt1InOneWeekend;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  //ngOnChanges, ngOnInit, ngDoChecks, ngAfterContentInit, ngAfterCcontentChecked, ngAfterViewinit, ngAfterViewChecked, ngOnDestroy

  ngOnInit(): void {
    this.http.get<Rt1InOneWeekend>(this.baseUrl + 'rt1inoneweekend').subscribe(result => {
      this.raytrace = result;
    }, error => console.error(error));
  }
}

