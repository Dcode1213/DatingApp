import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { CommonModule } from '@angular/common';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [CommonModule,TabsModule,TimeagoModule,NgxGalleryModule, MemberMessagesComponent]

})
export class MemberDetailComponent implements OnInit {
// member: Member | undefined;
  member: Member = {} as Member;
galleryOptions :NgxGalleryOptions[] = [];
galleryImages : NgxGalleryImage[] = [];

  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent
  activeTab?: TabDirective;
  messages: Message[] = [];

  constructor(private memberService: MembersService, private route :ActivatedRoute , private messageService: MessageService) { }

  ngOnInit(): void {
    // this.loadMember();
    this.route.data.subscribe({
      next: data => this.member = data['member']
    })

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })
    
    this.getImages();
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 200,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
      
    ]
    // this.galleryImages = this.getImages();
  }

  selectTab(heading: string){
    if(this.memberTabs){
      this.memberTabs.tabs.find(x => x.heading == heading)!.active = true;
    }
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if(this.activeTab.heading == 'Messages'){
      this.loadMessages();
    }
  }

  loadMessages(){
    if(this.member){
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }


  getImages(){
    if(!this.member) return [];          //returned an [] 
    const imageUrls = [];
    for(const photo of this.member.photos){
      imageUrls.push({
        small:photo.url,
        medium:photo.url,
        big:photo.url
      })
    }
    return imageUrls;
  }

  // loadMember(){
  //   const username = this.route.snapshot.paramMap.get('username')      //SNAPSHOT PENDING
  //   if(!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next : member => {
  //       this.member = member,
  //       this.galleryImages = this.getImages();
  //     }
  //   })
  // }

}
