import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  @Output() cancelRegister = new EventEmitter();
  model: any = {}

  constructor(private accoutService: AccountService) {
  }
  ngOnInit(){

  }
  register(){
    this.accoutService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: err => console.log(err)
    })
  }
  cancel(){
    this.cancelRegister.emit(false)
  }
}
