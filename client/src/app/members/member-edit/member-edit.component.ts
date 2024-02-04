import { Component, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { NgForm }   from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent {
  @ViewChild('editForm') editForm: NgForm | undefined;
  member: Member | undefined;
  user: User | null = null;
  protected city: string = "";
  protected country: string = "";
  constructor(private accountService: AccountService,
              private memberService: MembersService,
              private toastr: ToastrService,
              private router: Router) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
  }
  ngOnInit(){
    this.loadMember();
  }
  loadMember(){
    if(!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    })
  }

  updateMember(){
    if(this.member && this.city != '' && this.country != ''){
      this.member.adress = this.city + ', ' + this.country;
    }
    if(this.member == undefined) return;
    this.memberService.updateMember(this.member);
    this.router.navigateByUrl('/members');
    console.log(this.member);
    this.toastr.success('Update succesful', 'some-title', {positionClass: 'toast-top-center'});
  }
}
