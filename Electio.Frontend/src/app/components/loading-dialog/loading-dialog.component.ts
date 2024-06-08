import { Component } from '@angular/core';

@Component({
  selector: 'app-loading-dialog',
  template: `
    <h2 mat-dialog-title>Loading</h2>
    <mat-dialog-content>
      <p>Algorithm is running, please wait...</p>
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
