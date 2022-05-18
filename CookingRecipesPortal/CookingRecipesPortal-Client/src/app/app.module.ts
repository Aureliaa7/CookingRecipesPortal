import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { RegisterComponent } from './components/accounts/register/register.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { AccountService } from './services/account.service';
import { AppRoutingModule } from './app-routing.module';
import { LoginComponent } from './components/accounts/login/login.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { RecipeCardComponent } from './components/recipes/recipe-card/recipe-card.component';
import { RecipesListComponent } from './components/recipes/recipes-list/recipes-list.component';
import { SavedRecipesComponent } from './components/recipes/saved-recipes/saved-recipes.component';
import { UserRecipesComponent } from './components/recipes/user-recipes/user-recipes.component';
import { ImageCarouselComponent } from './components/image-carousel/image-carousel.component';
import { MatCarouselModule } from '@ngmodule/material-carousel';
import { MatIconModule } from '@angular/material/icon'
import { RecipeService } from './services/recipe.service';
import { AuthInterceptor } from './services/auth-interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    NavMenuComponent,
    HomeComponent,
    RecipeCardComponent,
    RecipesListComponent,
    SavedRecipesComponent,
    UserRecipesComponent,
    ImageCarouselComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatCarouselModule.forRoot(),
    MatIconModule
  ],
  providers: [
    AccountService,
    RecipeService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
