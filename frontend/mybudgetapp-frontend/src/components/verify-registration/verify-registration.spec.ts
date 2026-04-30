import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifyRegistration } from './verify-registration';

describe('VerifyRegistration', () => {
  let component: VerifyRegistration;
  let fixture: ComponentFixture<VerifyRegistration>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VerifyRegistration],
    }).compileComponents();

    fixture = TestBed.createComponent(VerifyRegistration);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
