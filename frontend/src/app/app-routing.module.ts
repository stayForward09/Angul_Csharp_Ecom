import { ViewProductComponent } from './Components/view-product/view-product.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './Components/home/home.component';
import { SearchComponent } from './Components/search/search.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'Search', component: SearchComponent },
  { path: 'View', component: ViewProductComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
