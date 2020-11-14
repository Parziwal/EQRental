import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { RentalService } from 'src/app/rental/rental.service';
import { CategoryService } from 'src/app/shared/services/category.service';
import { MyEquipmentsService } from '../my-equipments.service';
import { EquipmentPost } from './equipment-post';
import { mimeType } from './mime-type.validator';

@Component({
  selector: 'app-equipment-create',
  templateUrl: './equipment-create.component.html',
  styleUrls: ['./equipment-create.component.css']
})
export class EquipmentCreateComponent implements OnInit {

  equipmentForm: FormGroup;
  imagePreview: string;
  private mode = 'create';
  private equipmentId: number;
  categories: string[];

  constructor(private rentalService: RentalService,
    private myEquipmentService: MyEquipmentsService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.initForm();
    this.route.paramMap.subscribe((paramMap: ParamMap) => {
        this.categoryService.getCategories().subscribe(
          (categoriesData) => {
            this.categories = categoriesData;
            this.equipmentForm.patchValue({
              category: categoriesData[0]
            });
          }
        );

        if (paramMap.has('equipmentId')) {
            this.mode = 'edit';
            this.equipmentId = +paramMap.get('equipmentId');
            this.rentalService.getRentalEquipment(this.equipmentId).subscribe(equipmetData => {
                this.imagePreview = equipmetData.imagePath;
                this.equipmentForm.setValue({
                  name: equipmetData.name,
                  details: equipmetData.details,
                  pricePerDay: equipmetData.pricePerDay,
                  category: equipmetData.category,
                  image: equipmetData.imagePath
                });
            });
        } else {
            this.mode = 'create';
            this.equipmentId = null;
        }
    });
  }

  private initForm() {
    this.equipmentForm = new FormGroup({
      name: new FormControl(null, [Validators.required]),
      details: new FormControl(null, [Validators.required]),
      pricePerDay: new FormControl(null, [Validators.required]),
      category: new FormControl(null, [Validators.required]),
      image: new FormControl(null, [Validators.required], [mimeType])
    });
  }

  onSubmit() {
    this.equipmentForm.get('image').markAsTouched();
    if (this.equipmentForm.invalid) {
        return;
    }

    let editEquipmentObs: Observable<EquipmentPost>;
    if (this.mode === 'create') {
      editEquipmentObs = this.myEquipmentService.addEquipment(this.equipmentForm.value);
    } else {
      // editEquipmentObs = this.myEquipmentService.updatePost(this.postId, this.form.value.title, this.form.value.content, this.form.value.image);
    }

    editEquipmentObs.subscribe((response) => {
      console.log(response);
      this.router.navigate(['my-equipments', response]);
    });
  }

  onImagePicked(event: Event) {
    this.equipmentForm.get('image').markAsTouched();

    const file = (event.target as HTMLInputElement).files[0];
    this.equipmentForm.patchValue({image: file});
    this.equipmentForm.get('image').updateValueAndValidity();
    const reader = new FileReader();
    reader.onload = () => {
        this.imagePreview = reader.result as string;
    };
    reader.readAsDataURL(file);
  }

  onCancel() {

  }
}
