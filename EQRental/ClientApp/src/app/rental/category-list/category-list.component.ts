import { Component, OnInit } from '@angular/core';
import { CategoryService } from 'src/app/shared/services/category.service';
import { RentalService } from '../rental.service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {

  categories = new Array<{name: string, checked: boolean}>();

  constructor(private categoryService: CategoryService,
    private rentalService: RentalService) { }

  ngOnInit() {
    this.categoryService.getCategories().subscribe(
      (categoriesData) => {
        categoriesData.forEach(element => {
          this.categories.push({name: element, checked: false});
        });
      }
    );
  }

  onFilterChange(index: number) {
    this.categories[index].checked = !this.categories[index].checked;

    const checkedCategories = this.categories
    .filter(this.checkedCategoriesFilter)
    .map((element: {name: string, checked: boolean}) => {
      return element.name;
    });
    this.rentalService.filterChanged.next(checkedCategories);
  }

  private checkedCategoriesFilter(element, index, array) {
    return element.checked;
  }

}
