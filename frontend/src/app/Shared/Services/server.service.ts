import {
  IcartItemsAdd,
  ILogin,
  IOrderReq,
  IUserDetail,
  Signup,
} from './../Models/response';
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
  getCartItems(): Observable<Response> {
    return this.Http.get<Response>(this.BaseUrl + 'api/CartItems/getCartItems');
  }
  addCartItems(data: IcartItemsAdd): Observable<Response> {
    return this.Http.post<Response>(
      this.BaseUrl + 'api/CartItems/addItemtoCart',
      data
    );
  }
  patchCartItems(data: IcartItemsAdd): Observable<Response> {
    return this.Http.patch<Response>(
      this.BaseUrl + 'api/CartItems/updateCartItems',
      data
    );
  }

  deleteCartItem(id: string) {
    return this.Http.delete<Response>(
      this.BaseUrl + `api/CartItems/deleteCartItem/${id}`
    );
  }

  saveUserDetails(data: IUserDetail): Observable<Response> {
    return this.Http.post<Response>(
      this.BaseUrl + 'Admin/saveUserDetails',
      data
    );
  }

  getUserAddressDetails(): Observable<Response> {
    return this.Http.get<Response>(
      this.BaseUrl + 'Admin/getUserAddressDetails'
    );
  }

  createOrder(orderType: number, data: Array<IOrderReq>): Observable<Response> {
    return this.Http.post<Response>(
      this.BaseUrl + `api/Order/createOrder?orderType=${orderType}`,
      data
    );
  }
  verifyPayment(id: string, paymentReflink: string): Observable<Response> {
    return this.Http.get<Response>(
      `${this.BaseUrl}api/Order/verifyPayment/${id}/${paymentReflink}`
    );
  }
}
