import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService, Cliente } from '../../services/api.service';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-create-client',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    MatTableModule
  ],
  templateUrl: './create-client.component.html',
  styleUrls: ['./create-client.component.css']
})
export class CreateClientComponent implements OnInit {
  clientForm: FormGroup;
  clientes: Cliente[] = [];
  displayedColumns: string[] = ['id', 'nome', 'dataCadastro'];

  constructor(
    private fb: FormBuilder,
    private apiService: ApiService,
    private snackBar: MatSnackBar
  ) {
    this.clientForm = this.fb.group({
      nome: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadClientes();
  }

  loadClientes(): void {
    this.apiService.getAllClientes().subscribe(data => {
      this.clientes = data;
    });
  }

  onSubmit(): void {
    if (this.clientForm.valid) {
      this.apiService.createClient(this.clientForm.value).subscribe({
        next: () => {
          this.snackBar.open('Cliente cadastrado com sucesso!', 'Fechar', { duration: 3000 });
          this.clientForm.reset();
          this.loadClientes();
        },
        error: () => {
          this.snackBar.open('Erro ao cadastrar cliente.', 'Fechar', { duration: 3000 });
        }
      });
    }
  }
}
