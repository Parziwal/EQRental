import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';

import { HomeComponent } from './home/home.component';
import { MyEquipmentsComponent } from './my-equipments/my-equipments.component';
import { MyEquipmentsDetailsComponent } from './my-equipments/my-equipment-details/my-equipment-details.component';
import { MyEquipmentsListComponent } from './my-equipments/my-equipments-list/my-equipments-list.component';
import { RentEquipmentComponent } from './rental/rental-equipment-details/rent-equipment/rent-equipment.component';
import { RentalEquipmentDetailsComponent } from './rental/rental-equipment-details/rental-equipment-details.component';
import { RentalListComponent } from './rental/rental-list/rental-list.component';
import { RentalComponent } from './rental/rental.component';
import { RentedEquipmentDetailsComponent } from './rented-equipments/rented-equipment-details/rented-equipment-details.component';
import { RentedEquipmentsListComponent } from './rented-equipments/rented-equipments-list/rented-equipments-list.component';
import { RentedEquipmentsComponent } from './rented-equipments/rented-equipments.component';
import { EquipmentCreateComponent } from './my-equipments/equipment-create/equipment-create.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'rental', component: RentalComponent, canActivate: [AuthorizeGuard], children: [
    {path: '', component: RentalListComponent},
    {path: ':id', component: RentalEquipmentDetailsComponent},
    {path: ':id/rent', component: RentEquipmentComponent},
  ] },
  { path: 'my-equipments', component: MyEquipmentsComponent, canActivate: [AuthorizeGuard], children: [
    { path: '', component: MyEquipmentsListComponent },
    { path: 'create', component: EquipmentCreateComponent },
    { path: ':id', component: MyEquipmentsDetailsComponent }
  ]},
  { path: 'rented-equipments', component: RentedEquipmentsComponent, canActivate: [AuthorizeGuard], children: [
    {path: '', component: RentedEquipmentsListComponent},
    {path: ':id', component: RentedEquipmentDetailsComponent}
  ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
