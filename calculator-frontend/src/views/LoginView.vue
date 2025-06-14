<template>
  <div class="login">
    <h2>Вход</h2>
    <form @submit.prevent="login">
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Пароль" required />
      <button type="submit">Войти</button>
    </form>
    <p v-if="error" style="color: red">{{ error }}</p>
  </div>
  <router-link to="/register">Нет аккаунта? Зарегистрироваться</router-link>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import axios from "axios";
import { useRouter } from "vue-router";
import { useStore } from "vuex";

export default defineComponent({
  setup() {
    const email = ref("");
    const password = ref("");
    const error = ref("");
    const router = useRouter();
    const store = useStore();

    async function login() {
      error.value = "";
      try {
        const response = await axios.post(
          "https://localhost:7100/api/auth/login",
          {
            email: email.value,
            password: password.value,
          }
        );
        const token = response.data.token;
        store.commit("setToken", token);
        router.push("/calculator");
      } catch (e: any) {
        error.value = e.response?.data?.error || "Ошибка при входе";
      }
    }

    return { email, password, error, login };
  },
});
</script>
