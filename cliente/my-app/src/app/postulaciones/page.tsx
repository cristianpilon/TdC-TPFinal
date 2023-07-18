import Link from "next/link";
import Layout from "../layoutUser";

export default function Postulaciones(){
  return (
  <Layout userLayout={false}>
    <h1>Postulaciones</h1>
    <div className="contenedor">
      <Link href="/">Volver</Link>
    </div>
  </Layout>);
}