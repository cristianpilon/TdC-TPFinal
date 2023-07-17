"use client";
import { useState } from "react"
import { useRouter } from 'next/navigation';

export default function Home() {
  const [usuario, setUsuario] = useState<string>();
  const [password, setPassword] = useState<string>();
  const { push } = useRouter();
  const ingresarClick = () => {

    if (password === '123456') {
      if (usuario === 'usuarioAdmin') {
        push('/empleos')
        return;
      }  
    }
    if (usuario === 'usuario' ) {
        push('postulaciones')
        return;
    }

    alert('El usuario o la contraseña es incorrecta');
  }

  return (
    <main>
      <div className="imgDiv"> 
          <label>Gestión de currículums</label> 
          <form action="" method="post">
            <img className="imgLogin" src="home/user.png" alt="" />
            <div className="input1">
                <div className="Username">
                    <img className="imgUser" src="home/17004.png" alt="" />
                    <input 
                        type="text" 
                        id="user" 
                        name="user" 
                        value={usuario} onChange={e => setUsuario(e.target.value)}
                        placeholder="Username" required />
                </div>
            </div>

            <div className="input2">
                <div className="Username">
                    <img className="imgUser" src="home/security-system.png" alt="" />
                    <input type="password"
                        id="psw" 
                        name="psw" 
                        value={password} onChange={e => setPassword(e.target.value)}
                        placeholder="Introduza la contraseña" required />
                </div>
            </div>
            <label className="p1"><input type="checkbox" value=""/>Recordarme</label>
            <a className="p2" href="#">¿Olvidó contraseña?</a>
            <div className="botonera">
              <input type="submit" id="log" value="Ingresar" onClick={ingresarClick} />
              <input type="submit" id="log" value="Registrar" />
            </div>
          </form>
      </div>
    </main>
  )
}
