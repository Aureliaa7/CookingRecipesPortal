import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/accounts/login/login.component';
import { RegisterComponent } from './components/accounts/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { UserRecipesComponent } from './components/recipes/user-recipes/user-recipes.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'user-recipes', component: UserRecipesComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
