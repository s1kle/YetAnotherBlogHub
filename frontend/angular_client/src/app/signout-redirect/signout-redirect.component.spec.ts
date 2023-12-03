import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignoutRedirectComponent } from './signout-redirect.component';

describe('SignoutRedirectComponent', () => {
  let component: SignoutRedirectComponent;
  let fixture: ComponentFixture<SignoutRedirectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SignoutRedirectComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SignoutRedirectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
