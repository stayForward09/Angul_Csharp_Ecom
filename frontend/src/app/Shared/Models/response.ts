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

export interface ICartItems {
  Description: string;
  ID: string;
  Images: Array<string>;
  Part: string;
  Price: number;
  Qty: number;
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
