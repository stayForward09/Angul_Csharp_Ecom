export interface Response {
  Data: any;
  Succeeded: boolean;
  Errors: Array<string>;
  Message: string;
}

export interface Product {
  Description: string;
  ID: string;
  Images: Array<PartImages>;
  Part: string;
  Price: number;
}

export interface PartImages {
  IsTd: boolean;
  Url: string;
}

export interface ILogin {
  emailID: string;
  Password: string;
}

export interface ILoginUser {
  Name: string;
  emailID: string;
  usID: string;
}

export interface Signup {
  UsID: string;
  Fname: string;
  Mname: string;
  Lname: string;
  DOB: Date;
  EmailID: string;
  EmailConfirmed: boolean;
  Password: string;
  IsAdmin: boolean;
}

export interface IcartItemsAdd {
  CITId: string;
  CIPrid: string;
  CIQty: number;
  CIUsid: string;
}

export interface ICart {
  Cid: string;
  Pid: string;
  Pname: string;
  Price: number;
  Images: string;
  Disount: IcartDiscount;
  Qty: number;
}

export interface IcartDiscount {
  type: number;
  Did: string;
  Price: number;
}

export interface IOrderReq {
  prdId: string;
  DId: string | null;
  Qty: number;
  Cid: string;
}

export interface IUserDetail {
  CompanyName: string;
  CompanyCategory: string;
  Address: string;
}

export interface IRazorpayRes {
  razorpay_payment_id:string;
  razorpay_payment_link_id:string;
  razorpay_payment_link_status:string;
  razorpay_signature:string;
}