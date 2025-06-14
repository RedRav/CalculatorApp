<template>
  <div>
    <div style="display: flex; justify-content: space-between">
      <button @click="logout">Выйти</button>
    </div>
    <h2>Калькулятор</h2>
    <form @submit.prevent="calculate">
      <input v-model.number="A" type="number" step="any" required />
      <select v-model="operation" required>
        <option value="+">+</option>
        <option value="-">-</option>
        <option value="*">*</option>
        <option value="/">/</option>
        <option value="^">^</option>
        <option value="root">root</option>
      </select>
      <input v-model.number="B" type="number" step="any" required />
      <button type="submit">Вычислить</button>
    </form>
    <div v-if="result !== null">Результат: {{ result }}</div>
    <div v-if="error" style="color: red">Ошибка: {{ error }}</div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import axios from "axios";
import { useStore } from "vuex";
import { useRouter } from "vue-router";

export default defineComponent({
  setup() {
    const A = ref(0);
    const B = ref(0);
    const operation = ref("+");
    const result = ref<number | null>(null);
    const error = ref("");
    const store = useStore();
    const router = useRouter();

    async function calculate() {
      error.value = "";
      result.value = null;
      try {
        const token = store.state.token;
        const response = await axios.post(
          "https://localhost:7100/api/calculator",
          {
            A: A.value,
            B: B.value,
            operation: operation.value,
          },
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        result.value = response.data.result;
      } catch (e: any) {
        error.value = e.response?.data?.error || "Ошибка запроса";
      }
    }

    const logout = () => {
      store.commit("setToken", null);
      router.push("/login");
    };

    return { A, B, operation, calculate, result, error, logout };
  },
});
</script>
