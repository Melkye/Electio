import { Component } from '@angular/core';

@Component({
  selector: 'app-loading-dialog',
  template: `
    <h2 mat-dialog-title>Завантаження</h2>
    <mat-dialog-content>
      <p>Будь ласка, почекайтe</p>
    </mat-dialog-content>
  `,
  styles: [`
    mat-dialog-content {
      display: flex;
      align-items: center;
      justify-content: center;
      text-align: center;
    }
  `]
})
export class LoadingDialogComponent {}
