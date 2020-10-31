import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';

import { HomeComponent } from './home/home.component';
import { MyEquipmentsComponent } from './my-equipments/my-equipments.component';
import { RentalComponent } from './rental/rental.component';
import { RentedEquipmentDetailsComponent } from './rented-equipments/rented-equipment-details/rented-equipment-details.component';
import { RentedEquipmentsListComponent } from './rented-equipments/rented-equipments-list/rented-equipments-list.component';
import { RentedEquipmentsComponent } from './rented-equipments/rented-equipments.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'rental', component: RentalComponent, canActivate: [AuthorizeGuard] },
  { path: 'my-equipments', component: MyEquipmentsComponent, canActivate: [AuthorizeGuard] },
  { path: 'rented-equipments', component: RentedEquipmentsComponent, canActivate: [AuthorizeGuard], children: [
    {path: '', component: RentedEquipmentsListComponent},
    {path: ':id', component: RentedEquipmentDetailsComponent}
  ]},
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
