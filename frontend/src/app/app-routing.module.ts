import { PlaceOrderComponent } from './Components/place-order/place-order.component';
import { AccountComponent } from './Components/account/account.component';
import { VerifyPaymentComponent } from './Components/verify-payment/verify-payment.component';
import { ViewProductComponent } from './Components/view-product/view-product.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { SearchComponent } from './Components/search/search.component';
import { CartComponent } from './Components/cart/cart.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'Search', component: SearchComponent },
  { path: 'View', component: ViewProductComponent },
  { path: 'Cart', component: CartComponent },
  { path: 'verifyPayment', component: VerifyPaymentComponent },
  { path: 'account', component: AccountComponent },
  { path: 'Placeorder', component: PlaceOrderComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
