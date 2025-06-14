<template>
  <router-view />
</template>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}

body {
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  background: #f4f6f8;
  margin: 0;
  padding: 20px;
  color: #333;
}

.result-wrapper {
  display: flex;
  justify-content: center;
  margin-top: 15px;
}

.result-success {
  padding: 10px 15px;
  background-color: #e6ffed;
  border: 1px solid #4caf50;
  border-radius: 8px;
  font-weight: 700;
  font-size: 1.2rem;
  color: #2e7d32;
  user-select: text;
}

button {
  background-color: #4a90e2;
  color: white;
  border: none;
  padding: 10px 16px;
  font-size: 1rem;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s ease;
  margin-top: 10px;
}

button:hover {
  background-color: #357abd;
}

input[type="number"],
select {
  padding: 8px 10px;
  font-size: 1rem;
  border-radius: 4px;
  border: 1px solid #ccc;
  margin-right: 8px;
  width: 120px;
}

h2,
h3 {
  color: #222;
  margin-bottom: 10px;
}

ul {
  padding-left: 20px;
  max-height: 580px;
  overflow-y: auto;
  background: white;
  border: 1px solid #ddd;
  border-radius: 4px;
}

li {
  padding: 6px 8px;
  border-bottom: 1px solid #eee;
}

li:last-child {
  border-bottom: none;
}

.log-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 6px 8px;
  border-bottom: 1px solid #eee;
}

.log-timestamp {
  flex: 1;
  text-align: left;
  color: #666;
  font-size: 0.85rem;
  white-space: nowrap;
}

.log-expression {
  flex: 1;
  text-align: center;
  font-weight: 600;
}

.log-result {
  flex: 1;
  text-align: right;
  font-weight: 700;
}

.log-result.error {
  color: red;
}

.container {
  max-width: 400px;
  margin: 60px auto;
  padding: 30px 25px;
  border: 1px solid #ccc;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgb(0 0 0 / 0.1);
  background-color: #fff;
  text-align: center;
  font-family: Arial, sans-serif;
}

.form input {
  display: block;
  width: 100%;
  padding: 12px 10px;
  margin: 12px -10px;
  font-size: 16px;
  border-radius: 6px;
  border: 1px solid #bbb;
  transition: border-color 0.3s;
}
</style>
