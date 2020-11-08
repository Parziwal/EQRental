import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { RentalComponent } from './rental/rental.component';
import { MyEquipmentsComponent } from './my-equipments/my-equipments.component';
import { AppRoutingModule } from './app-routing.module';
import { ErrorComponent } from './error/error.component';
import { RentedEquipmentsListComponent } from './rented-equipments/rented-equipments-list/rented-equipments-list.component';
import { RentedEquipmentDetailsComponent } from './rented-equipments/rented-equipment-details/rented-equipment-details.component';
import { RentedEquipmentsComponent } from './rented-equipments/rented-equipments.component';
import { RentalListComponent } from './rental/rental-list/rental-list.component';
import { CategoryListComponent } from './rental/rental-list/category-list/category-list.component';
import { RentalEquipmentDetailsComponent } from './rental/rental-equipment-details/rental-equipment-details.component';
import { RentEquipmentComponent } from './rental/rental-equipment-details/rent-equipment/rent-equipment.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RentalComponent,
    MyEquipmentsComponent,
    ErrorComponent,
    RentedEquipmentsComponent,
    RentedEquipmentsListComponent,
    RentedEquipmentDetailsComponent,
    RentalListComponent,
    CategoryListComponent,
    RentalEquipmentDetailsComponent,
    RentEquipmentComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    ApiAuthorizationModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
