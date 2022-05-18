import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { RecipeDetails } from '../../../interfaces/recipe-details.interface';
import { AccountService } from '../../../services/account.service';

@Component({
  selector: 'app-recipe-card',
  templateUrl: './recipe-card.component.html',
  styleUrls: ['./recipe-card.component.css']
})
export class RecipeCardComponent implements OnInit {

  @Input()
  recipe!: RecipeDetails;

  isRecipeAuthor: boolean = false;

  @Output()
  editRecipe: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  deleteRecipe: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  saveRecipe: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  likeRecipe: EventEmitter<string> = new EventEmitter<string>();

  isUserLoggedIn!: boolean;

  constructor(private userService: AccountService) { }

  ngOnInit(): void {
    this.isUserLoggedIn = !!this.userService.getCurrentUserId();
    this.isRecipeAuthor = this.userService.getCurrentUserId() === this.recipe.authorId;
    console.log(`isRecipeAuthor: ${this.isRecipeAuthor}, currUserId: ${this.userService.getCurrentUserId()}
    recipeAuthorId: ${this.recipe.authorId}`);
  }
}
