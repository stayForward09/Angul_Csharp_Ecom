import { DataService } from './Shared/Services/data.service';
import { Component, OnInit, HostListener, ViewChild } from '@angular/core';
import { ServerService } from './Shared/Services/server.service';
import {
  Response,
  ILogin,
  ILoginUser,
  Signup,
  ICart,
} from './Shared/Models/response';
import { FormControl } from '@angular/forms';
import { debounceTime, Observable, empty, retry } from 'rxjs';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { DialogComponent } from './Shared/Components/dialog/dialog.component';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Home';
  isError = false;
  SearchTerm: FormControl = new FormControl();
  SearchResult: any[] = [];
  Showdropdown: boolean = false;
  showProfileManu: boolean = false;
  $cartItems: Observable<Array<ICart>> = empty();

  LogedIn: boolean = false;
  LoggedUser: ILoginUser = <ILoginUser>{};
  LoginForm: ILogin = <ILogin>{};
  @ViewChild('dialog', { static: true }) dialogElement!: DialogComponent;
  formNo: number = 0;
  signUpForm: Signup = <Signup>{};
  private theme: string = 'l';

  constructor(
    private server: ServerService,
    private route: Router,
    private TitleService: Title,
    private dataServer: DataService
  ) {
    this.SetTitle(this.title);
    this.dataServer.title.subscribe((x) => {
      if (x.trim() !== '') {
        this.SetTitle(x);
      }
    });
    this.$cartItems = this.dataServer.cartItems.asObservable();
    let mode = window.localStorage.getItem('eSt') ?? 'l';
    this.changeTheme(mode);
  }

  ngOnInit(): void {
    this.SearchTerm.valueChanges
      .pipe(debounceTime(1000))
      .subscribe((x: string) => {
        if (x.trim().length > 3) {
          this.GetSrchResult(x);
        }
      });
    this.CheckLogedIn();
  }

  GetSrchResult(keyWord: string) {
    this.server.GetPartsrchResult(keyWord).subscribe((x: Response) => {
      this.SearchResult = x.Data;
      this.Showdropdown = this.SearchResult.length > 0;
    });
  }

  SetSrchBoxvalue(data: any) {
    this.SearchTerm.setValue(data.prdName);
  }

  @HostListener('document:click', ['$event'])
  clickedOut(event: any) {
    if (event.target.id === 'txtsrch') {
      this.Showdropdown = false;
      this.showProfileManu = false;
    } else if (event.target.id === 'imgprofile') {
      this.Showdropdown = false;
      this.showProfileManu = true;
    } else {
      this.Showdropdown = false;
      this.showProfileManu = false;
    }
  }

  SearchClick() {
    if (this.SearchTerm.value && this.SearchTerm.value.trim() != '') {
      this.route.navigate(['/Search'], {
        queryParams: { q: this.SearchTerm.value },
      });
    } else {
      this.SearchTerm.setValue('');
    }
  }

  SetTitle(title: string) {
    this.TitleService.setTitle(title + ' - Part.Shop');
  }

  HighlightText(value: string) {
    let rGx = new RegExp(this.SearchTerm.value, 'ig');
    return (
      '&nbsp;' +
      value
        .toLowerCase()
        .replace(rGx, `<b>${this.SearchTerm.value.toLowerCase()}</b>`)
    );
  }

  Login(formData: any) {
    this.server.LoginUser(this.LoginForm).subscribe(
      (x: Response) => {
        console.log(x);
        if (x.Succeeded) {
          window.localStorage.setItem('psToken', x.Data.Token);
          this.dialogElement.Close();
          formData.form.reset();
          this.getUserDetails();
        }
      },
      (err: HttpErrorResponse) => {
        if (err.status === 400) {
          console.log(err.error.Message);
          if (err.error.Message.toLowerCase().includes('password')) {
            formData.form.controls['username'].setErrors({
              invaliduspass: true,
            });
            formData.form.controls['password'].setErrors({
              invaliduspass: true,
            });
          } else if (err.error.Message.toLowerCase().includes('username')) {
            formData.form.controls['username'].setErrors({
              invalidusername: true,
            });
          }
        }
      }
    );
  }

  getUserDetails() {
    this.server.getUserdetails().subscribe(
      (res: Response) => {
        this.LoggedUser = res.Data as ILoginUser;
        this.LogedIn = true;
        this.dataServer.ShareLoginUser(this.LoggedUser);
        this.dataServer.ShareisLogged(this.LogedIn);
        this.dataServer.getCartDetails();
      },
      (err) => {
        console.log(err);
        this.LogedIn = false;
        this.dataServer.ShareLoginUser(<ILoginUser>{});
        this.dataServer.ShareisLogged(this.LogedIn);
        retry(2);
      }
    );
  }

  CheckLogedIn() {
    let token = window.localStorage.getItem('psToken');
    if (token) {
      this.getUserDetails();
    }
  }

  // getUserdetailsRetry() {
  //   this.server
  //     .getUserdetails()
  //     .pipe(delay(1000), retry(2))
  //     .subscribe((x) => {
  //       console.log(x);
  //     });
  // }

  createAccount() {
    this.signUpForm.IsAdmin = false;
    this.server.AddUser(this.signUpForm).subscribe(
      (x: Response) => {
        if (x.Succeeded) {
          window.localStorage.setItem('psToken', x.Data.Token);
          this.dialogElement.Close();
          this.getUserDetails();
        }
      },
      (err) => {
        console.log(err);
      }
    );
  }

  toggleMode() {
    this.theme = document.body.classList.contains('dark-mode') ? 'l' : 'd';
    window.localStorage.setItem('eSt', this.theme);
    document.body.classList.toggle('dark-mode');
  }
  changeTheme(val: string) {
    let isDarkMode = document.body.classList.contains('dark-mode');
    if (!isDarkMode && val === 'd') {
      document.body.classList.toggle('dark-mode');
    }
  }
}
