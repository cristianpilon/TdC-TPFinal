import Link from "next/link";

export default function Home() {
  return (
    <main>
      <ul>
        <li>
          <Link href="/empleos">Empleos</Link>
        </li>
        <li>
          <Link href="/postulaciones">Postulaciones</Link>
        </li>
      </ul>      
    </main>
  )
}
