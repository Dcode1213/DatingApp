import { NgModule } from '@angular/core';
import { GuardsCheckEnd, RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-detailed.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { adminGuard } from './_guards/admin.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: '', 
    runGuardsAndResolvers:'always',
    canActivate: [AuthGuard],
    children:[
      {path: 'members', component: MemberListComponent},
      {path: 'members/:username', component: MemberDetailComponent,resolve: {member: MemberDetailedResolver}},
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [adminGuard]},
      {path: 'member/edit',component:MemberEditComponent,canDeactivate:[PreventUnsavedChangesGuard]},
    ]
  },
  {path: 'errors',component: TestErrorComponent},
  {path: 'not-found',component: NotFoundComponent},
  {path: 'server-error',component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'},
];
  
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
