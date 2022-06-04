import { ILogin, Signup } from './../Models/response';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Response } from '../Models/response';

@Injectable({
  providedIn: 'root',
})
export class ServerService {
  private BaseUrl: string = environment.BaseUrl;

  constructor(private Http: HttpClient) {}

  GetPartsDetailed(): Observable<Response> {
    return this.Http.get<Response>(this.BaseUrl + 'Admin/GetPartsDetailed');
  }
  GetPartsrchResult(data: string): Observable<Response> {
    return this.Http.get<Response>(
      this.BaseUrl + `Admin/SearchProduct/${data}`
    );
  }

  getPartResult(
    data: string,
    pagenumber: number,
    pageSize: number
  ): Observable<Response> {
    return this.Http.get<Response>(
      this.BaseUrl + `Admin/SearchPrd/${data}/${pagenumber}/${pageSize}`
    );
  }
  GetPartbyID(data: string): Observable<Response> {
    return this.Http.get<Response>(this.BaseUrl + `Admin/GetPartbyID/${data}`);
  }
  getHomeDetails(): Observable<Response> {
    return this.Http.get<Response>(this.BaseUrl + `Admin/getHomeDetails`);
  }
  LoginUser(data: ILogin): Observable<Response> {
    return this.Http.post<Response>(this.BaseUrl + 'Admin/LoginUser', data);
  }
  getUserdetails(): Observable<Response> {
    return this.Http.get<Response>(this.BaseUrl + 'Admin/getUserdetails');
  }
  AddUser(data: Signup): Observable<Response> {
    return this.Http.post<Response>(this.BaseUrl + 'Admin/AddUser', data);
  }
}
