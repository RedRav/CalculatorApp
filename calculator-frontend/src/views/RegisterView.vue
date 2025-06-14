<template>
  <div>
    <h2>Регистрация</h2>
    <form @submit.prevent="register">
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Пароль" required />
      <button type="submit">Зарегистрироваться</button>
    </form>
    <p v-if="error" style="color: red">{{ error }}</p>
  </div>
  <router-link to="/login">Уже есть аккаунт? Войти</router-link>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";

export default defineComponent({
  setup() {
    const email = ref("");
    const password = ref("");
    const error = ref("");
    const router = useRouter();

    async function register() {
      error.value = "";
      try {
        await axios.post("https://localhost:7100/api/auth/register", {
          email: email.value,
          password: password.value,
        });
        router.push("/login");
      } catch (e: any) {
        error.value = e.response?.data?.error || "Ошибка при регистрации";
      }
    }

    return { email, password, register, error };
  },
});
</script>
