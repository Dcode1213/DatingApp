import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users : User[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();   //is used to pass data to that role modal component
  availableRoles = [
    'Admin',
    'Moderator',    //id many that get from server
    'Member'
  ]

  constructor(private adminService: AdminService,private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }
  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe({
      next: users => this.users = users                           //primary code
      // next: user =>{
      //   if(user) this.users = user;
      // }
    })
  }

  openRolesModal(user: User){
    const config ={
      class: 'modal-dialog-centered',
      initialState: {
        username : user.username,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        const selectedRoles = this.bsModalRef.content?.selectedRoles;
        if(!this.arrayEqual(selectedRoles,user.roles)){
          this.adminService.updateUserRoles(user.username,selectedRoles!).subscribe({
            next: roles => user.roles = roles
            // next: roles =>{
            //   user.roles = roles
            // }
          })
        }
      }
    })

    // const initialState: ModalOptions = {
    //   initialState: {
    //     list: [
    //       'Do thing',
    //       'Another thing',
    //       'Something else'
    //     ],
    //     title: 'test modal'
    //   }
    // }
    // this.bsModalRef = this.modalService.show(RolesModalComponent,initialState);
    // this.bsModalRef.content!.closeBtnName = 'Close';
  }

  private arrayEqual(arr1: any,arr2: any){
    return JSON.stringify(arr1.sort()) == JSON.stringify(arr2.sort());
  }

}
