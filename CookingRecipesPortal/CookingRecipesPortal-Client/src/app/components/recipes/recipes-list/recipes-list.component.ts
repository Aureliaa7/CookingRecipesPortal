import { Component, Input } from '@angular/core';
import { RecipeDetails } from '../../../interfaces/recipe-details.interface';

@Component({
  selector: 'app-recipes-list',
  templateUrl: './recipes-list.component.html',
  styleUrls: ['./recipes-list.component.css']
})
export class RecipesListComponent {

  @Input()
  recipes!: RecipeDetails[];
}
