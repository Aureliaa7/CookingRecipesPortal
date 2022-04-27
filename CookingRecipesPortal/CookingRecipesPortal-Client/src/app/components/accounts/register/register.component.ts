import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ResponseStatusCodes } from '../../../enums/response-status-codes.enum';
import { UserRegistration } from '../../../models/user-registration';
import { AccountService } from '../../../services/account.service';

///
/* the password must have:
- at least one upper case: (?=.*?[A-Z])
- at least one lower case: (?=.*?[a-z])
- at least one number: (?=.*?[0-9])
- at least one special character: (?=.*?[#?!@$%^&*-])
- and minimum eight in length .{8,} (with the anchors)

read more about this https://www.regular-expressions.info/lookaround.html
*/
const passwordValidator = Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$');

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  registerForm = this.formBuilder.group({
    firstName: new FormControl('', [Validators.required, Validators.minLength(3)]),
    lastName: new FormControl('', [Validators.required, Validators.minLength(3)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, passwordValidator])
  });

  constructor(
    private accountService: AccountService,
    private router: Router,
    private formBuilder: FormBuilder) { }

  register() {
    if (this.registerForm.invalid) {
      return;
    }

    let user = new UserRegistration(
      this.registerForm.value.firstName,
      this.registerForm.value.lastName,
      this.registerForm.value.email,
      this.registerForm.value.password);
    this.accountService.register(user).subscribe(
      () => this.router.navigate(["/login"]),
      err => {
        if (err.status == ResponseStatusCodes.Conflict) {
          this.registerForm.reset();
        }
      });
  }
}
